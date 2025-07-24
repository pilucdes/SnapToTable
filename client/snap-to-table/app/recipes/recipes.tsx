import {useGetAllRecipes} from "@/features/recipes/hooks/useRecipe";
import {useLocalSearchParams} from "expo-router";
import {ActivityIndicator, Pressable, SafeAreaView, Text, View} from "react-native";

export default function RecipeListScreen() {

    const {recipeAnalysisId} = useLocalSearchParams<{ recipeAnalysisId: string }>();
    const {data, isLoading, error} = useGetAllRecipes({recipeAnalysisId, filter: "", pageSize: 20, page: 1});

    return (
        <SafeAreaView className="flex-1 bg-zinc-900">
            <View className="flex-1 items-center justify-center gap-3 p-8">
                {
                    isLoading ? (
                        <ActivityIndicator size="large"/>
                    ) : (data?.items.map((recipe, index) => (
                        <Pressable id={recipe.id}
                                   className="w-full max-w-96 shadow-sm dark:bg-gray-800 rounded-lg p-6 hover:dark:bg-gray-700">
                            <Text className="text-2xl dark:text-white font-bold">{recipe.name}</Text>
                            <Text className="text-xl dark:text-white">{recipe.category}</Text>
                            {recipe.ingredients.map((ingredient) => (
                                <Text className="text-md dark:text-white">• {ingredient}</Text>
                            ))}
                        </Pressable>
                    )))
                }

                {error && (
                    <Text className="text-red-400 mt-6 text-center">
                        {error.message}
                    </Text>
                )}
            </View>

        </SafeAreaView>
    );
}