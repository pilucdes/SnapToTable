import {useLocalSearchParams} from "expo-router";
import {useGetRecipeById} from "@/features/recipes/hooks/useRecipe";
import {SafeAreaView, ScrollView, View, Text, ActivityIndicator, Image} from "react-native";
import tw from "@/lib/tailwind";
import { SectionCard } from "@/features/recipes/components/SectionCard";
import { Feather } from "@expo/vector-icons";
import { InfoPill } from "@/features/recipes/components/InfoPill";

export default function RecipeDetailScreen() {
    const { id } = useLocalSearchParams<{ id: string }>();
    const { data, isLoading, error } = useGetRecipeById(id);

    if (isLoading) {
        return (
            <SafeAreaView style={tw`flex-1 bg-gray-100 dark:bg-zinc-900 justify-center items-center`}>
                <ActivityIndicator size="large" color={tw.color('blue-500')} />
            </SafeAreaView>
        );
    }

    if (error) {
        return (
            <SafeAreaView style={tw`flex-1 bg-gray-100 dark:bg-zinc-900 justify-center items-center p-6`}>
                <Text style={tw`text-red-500 dark:text-red-400 text-center`}>
                    {error.message}
                </Text>
            </SafeAreaView>
        )
    }

    // Calculate total time
    const totalTime = (data?.prepTimeInMinutes || 0) + (data?.cookTimeInMinutes || 0) + (data?.additionalTimeInMinutes || 0);

    return (
        <SafeAreaView style={tw`flex-1 bg-gray-100 dark:bg-zinc-900`}>
            <ScrollView>
                {/* 1. Hero Image */}
                <Image
                    source={{ uri: 'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?q=80&w=2960' }}
                    style={tw`w-full h-64`}
                />

                <View style={tw`p-6`}>
                    {/* 2. Header Section */}
                    <View style={tw`mb-6`}>
                        <Text style={tw`text-3xl font-extrabold text-zinc-900 dark:text-white tracking-tight`}>
                            {data?.name}
                        </Text>
                        <Text style={tw`text-base mt-2 px-3 py-1 bg-blue-100 dark:bg-blue-900/50 text-blue-800 dark:text-blue-300 rounded-full self-start`}>
                            {data?.category}
                        </Text>
                    </View>

                    {/* 3. Key Information Pills */}
                    <View style={tw`flex-row justify-around items-center bg-white dark:bg-zinc-800 p-4 rounded-xl shadow-sm`}>
                        <InfoPill iconName="users" label="Serves" value={`${data?.servings}`} />
                        <InfoPill iconName="clock" label="Prep" value={`${data?.prepTimeInMinutes} min`} />
                        <InfoPill iconName="thermometer" label="Cook" value={`${data?.cookTimeInMinutes} min`} />
                        <InfoPill iconName="bar-chart-2" label="Total" value={`${totalTime} min`} />
                    </View>

                    {/* 4. Ingredients Section */}
                    {data?.ingredients && data.ingredients.length > 0 && (
                        <SectionCard title="Ingredients">
                            {data.ingredients.map((ingredient) => (
                                <View key={ingredient} style={tw`flex-row items-center mb-2`}>
                                    <Feather name="circle" style={tw`text-blue-500 mr-2`} />
                                    <Text style={tw`text-base text-gray-700 dark:text-gray-300 flex-1`}>{ingredient}</Text>
                                </View>
                            ))}
                        </SectionCard>
                    )}

                    {/* 5. Directions Section */}
                    {data?.directions && data.directions.length > 0 && (
                        <SectionCard title="Directions">
                            {data.directions.map((direction, index) => (
                                <View key={direction} style={tw`flex-row mb-3`}>
                                    <Text style={tw`text-base font-bold text-blue-500 mr-3`}>{index + 1}.</Text>
                                    <Text style={tw`text-base text-gray-700 dark:text-gray-300 flex-1`}>{direction}</Text>
                                </View>
                            ))}
                        </SectionCard>
                    )}

                    {/* 6. Notes Section (Conditional) */}
                    {data?.notes && data.notes.length > 0 && (
                        <SectionCard title="Notes">
                            {data.notes.map((note) => (
                                <View key={note} style={tw`flex-row items-start mb-2`}>
                                    <Feather name="info" size={16} style={tw`text-gray-500 dark:text-gray-400 mr-2 mt-1`} />
                                    <Text style={tw`text-base text-gray-600 dark:text-gray-400 flex-1`}>{note}</Text>
                                </View>
                            ))}
                        </SectionCard>
                    )}
                </View>
                {/* Add some padding at the bottom of the scroll view */}
                <View style={tw`h-6`} />
            </ScrollView>
        </SafeAreaView>
    );
}