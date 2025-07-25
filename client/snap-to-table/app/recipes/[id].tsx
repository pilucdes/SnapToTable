import {useLocalSearchParams} from "expo-router";
import {useGetRecipeById} from "@/features/recipes/hooks/useRecipe";
import {SafeAreaView, ScrollView, View, Text, ActivityIndicator} from "react-native";
import tw from "@/lib/tailwind";

export default function RecipeDetailScreen() {

    const {id} = useLocalSearchParams<{ id: string }>();
    const {data, isLoading, error} = useGetRecipeById(id);

    return (
        <SafeAreaView style={tw`flex-1 bg-gray-100 dark:bg-zinc-900`}>
            {isLoading ? (
                <ActivityIndicator size="large" style={tw`mt-20`}/>
            ) : (
                <ScrollView contentContainerStyle={tw`items-center gap-4 p-6`}>
                    <Text style={tw`text-2xl font-bold text-zinc-900 dark:text-white`}>{data?.name}</Text>
                    <Text style={tw`text-lg mt-1 text-gray-600 dark:text-gray-300`}>{data?.category}</Text>
                    <Text style={tw`text-lg mt-1 text-gray-600 dark:text-gray-300`}>Serving {data?.servings}</Text>
                    
                    <Text style={tw`text-lg mt-1 text-gray-600 dark:text-gray-300`}>{data?.prepTimeInMinutes}</Text>
                    <Text style={tw`text-lg mt-1 text-gray-600 dark:text-gray-300`}>{data?.cookTimeInMinutes}</Text>
                    <Text style={tw`text-lg mt-1 text-gray-600 dark:text-gray-300`}>{data?.additionalTimeInMinutes}</Text>

                    
                    <View style={tw`mt-4 border-t border-gray-200 dark:border-gray-700 pt-4`}>
                        {data?.ingredients.slice(0, 5).map((ingredient) => (
                            <Text key={ingredient}
                                  style={tw`text-sm text-gray-500 dark:text-gray-400`}>• {ingredient}</Text>
                        ))}
                    </View>
                    <View style={tw`mt-4 border-t border-gray-200 dark:border-gray-700 pt-4`}>
                        {data?.directions.slice(0, 5).map((direction) => (
                            <Text key={direction}
                                  style={tw`text-sm text-gray-500 dark:text-gray-400`}>• {direction}</Text>
                        ))}
                    </View>
                    <View style={tw`mt-4 border-t border-gray-200 dark:border-gray-700 pt-4`}>
                        {data?.notes.slice(0, 5).map((note) => (
                            <Text key={note}
                                  style={tw`text-sm text-gray-500 dark:text-gray-400`}>• {note}</Text>
                        ))}
                    </View>
                </ScrollView>
            )}
            
            {error && (
                <Text style={tw`text-red-400 mt-6 text-center`}>
                    {error.message}
                </Text>
            )}
        </SafeAreaView>
    );
}