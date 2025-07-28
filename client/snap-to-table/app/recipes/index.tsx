import { RecipeCard } from "@/features/recipes/components/RecipeCard";
import { useGetAllRecipes } from "@/features/recipes/hooks/useRecipe";
import { useLocalSearchParams } from "expo-router";
import { ActivityIndicator, ScrollView } from "react-native";
import tw from "@/lib/tailwind";
import {ThemeSafeAreaView, ThemeText } from "@/features/common/components";

export default function RecipesScreen() {
    const { recipeAnalysisId } = useLocalSearchParams<{ recipeAnalysisId: string }>();
    const { data, isLoading, error } = useGetAllRecipes({ recipeAnalysisId, filter: "", pageSize: 20, page: 1 });

    return (
        <ThemeSafeAreaView>
            
            <ScrollView contentContainerStyle={tw`items-center gap-4 p-6`}>
                {isLoading ? (
                    <ActivityIndicator size="large" style={tw`mt-20`} />
                ) : (
                    data?.items.map((recipe) => (
                        <RecipeCard key={recipe.id} recipe={recipe} />
                    ))
                )}

                {error && (
                    <ThemeText style={tw`mt-6 text-center`}>
                        {error.message}
                    </ThemeText>
                )}
            </ScrollView>
        </ThemeSafeAreaView>
    );
}