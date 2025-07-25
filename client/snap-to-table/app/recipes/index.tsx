import { RecipeCard } from "@/features/recipes/components/RecipeCard";
import { useGetAllRecipes } from "@/features/recipes/hooks/useRecipe";
import { useLocalSearchParams } from "expo-router";
import { ActivityIndicator, SafeAreaView, Text, ScrollView } from "react-native";
import tw from "@/lib/tailwind";

export default function RecipesScreen() {
    const { recipeAnalysisId } = useLocalSearchParams<{ recipeAnalysisId: string }>();
    const { data, isLoading, error } = useGetAllRecipes({ recipeAnalysisId, filter: "", pageSize: 20, page: 1 });

    return (
        <SafeAreaView style={tw`flex-1 bg-gray-100 dark:bg-zinc-900`}>
            
            <ScrollView contentContainerStyle={tw`items-center gap-4 p-6`}>
                {isLoading ? (
                    <ActivityIndicator size="large" style={tw`mt-20`} />
                ) : (
                    data?.items.map((recipe) => (
                        <RecipeCard key={recipe.id} recipe={recipe} />
                    ))
                )}

                {error && (
                    <Text style={tw`text-red-400 mt-6 text-center`}>
                        {error.message}
                    </Text>
                )}
            </ScrollView>
        </SafeAreaView>
    );
}