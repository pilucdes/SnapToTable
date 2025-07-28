import {useLocalSearchParams} from "expo-router";
import {useGetRecipeById} from "@/features/recipes/hooks/useRecipe";
import {SafeAreaView, ScrollView, View, Text, ActivityIndicator, Image} from "react-native";
import tw from "@/lib/tailwind";
import { SectionCard } from "@/features/recipes/components/SectionCard";
import { InfoPill } from "@/features/recipes/components/InfoPill";
import {Icon, ThemeSafeAreaView, ThemeText } from "@/features/common/components";

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
        <ThemeSafeAreaView>
            <ScrollView>
    
                <Image
                    source={{ uri: 'https://images.unsplash.com/photo-1546069901-ba9599a7e63c?q=80&w=2960' }}
                    style={tw`w-full h-64`}
                />

                <View style={tw`p-6`}>
       
                    <View style={tw`mb-6`}>
                        <ThemeText style={tw`text-3xl font-extrabold tracking-tight`}>
                            {data?.name}
                        </ThemeText>
                        <ThemeText style={tw`mt-2 px-3 py-1 rounded-full self-start`}>
                            {data?.category}
                        </ThemeText>
                    </View>

 
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
                                    <Icon name="circle" style={tw`mr-2`} />
                                    <ThemeText style={tw`flex-1`}>{ingredient}</ThemeText>
                                </View>
                            ))}
                        </SectionCard>
                    )}

                    {/* 5. Directions Section */}
                    {data?.directions && data.directions.length > 0 && (
                        <SectionCard title="Directions">
                            {data.directions.map((direction, index) => (
                                <View key={direction} style={tw`flex-row mb-3`}>
                                    <ThemeText style={tw`font-bold mr-3`}>{index + 1}.</ThemeText>
                                    <ThemeText style={tw`flex-1`}>{direction}</ThemeText>
                                </View>
                            ))}
                        </SectionCard>
                    )}
                    
                    {data?.notes && data.notes.length > 0 && (
                        <SectionCard title="Notes">
                            {data.notes.map((note) => (
                                <View key={note} style={tw`flex-row items-start mb-2`}>
                                    <Icon name="info" size={16} style={tw`mr-2 mt-1`} />
                                    <ThemeText style={tw`flex-1`}>{note}</ThemeText>
                                </View>
                            ))}
                        </SectionCard>
                    )}
                </View>
                
                <View style={tw`h-6`} />
            </ScrollView>
        </ThemeSafeAreaView>
    );
}