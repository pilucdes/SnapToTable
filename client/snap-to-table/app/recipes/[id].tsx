import {useLocalSearchParams} from "expo-router";
import {useGetRecipeById} from "@/features/recipes/hooks/useRecipe";
import {SafeAreaView, ScrollView, View, Text, ActivityIndicator, Image} from "react-native";
import tw from "@/lib/tailwind";
import {SectionCard} from "@/features/recipes/components/SectionCard";
import {InfoPill} from "@/features/recipes/components/InfoPill";
import {ThemeSafeAreaView, ThemeText} from "@/features/common/components";
import { RecipeImage } from "@/features/recipes/components";
import { colorTheme } from "@/features/common/themes";

export default function RecipeDetailScreen() {
    const {id} = useLocalSearchParams<{ id: string }>();
    const {data, isLoading, error} = useGetRecipeById(id);
    const getDisplayMinutes = (dataMinutes: number | null | undefined) => {

        if (!dataMinutes) {
            return "";
        }

        return `${dataMinutes} min`;
    }

    if (isLoading) {
        return (
            <SafeAreaView style={tw`flex-1 bg-gray-100 dark:bg-zinc-900 justify-center items-center`}>
                <ActivityIndicator size="large" color={tw.color('blue-500')}/>
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

    return (
        <ThemeSafeAreaView>
            <ScrollView >

                <RecipeImage
                    url={data?.url}
                    style={tw`h-64`}
                />

                <View style={tw`p-6 lg:px-100 gap-4`}>


                    <View style={tw``}>
                        <ThemeText variant="title" color={colorTheme.accent.opt3} style={tw`font-bold`}>
                            {data?.name}
                        </ThemeText>
                        <ThemeText variant="heading" style={tw`mt-2 px-3 py-1 rounded-full self-start font-bold`}>
                            {data?.category}
                        </ThemeText>
                    </View>


                    <View style={tw`flex-row justify-around items-center`}>
                        <InfoPill iconName="users" label="Serves" value={`${data?.servings}`}/>
                        <InfoPill iconName="clock" label="Prep" value={getDisplayMinutes(data?.prepTimeInMinutes)}/>
                        <InfoPill iconName="thermometer" label="Cook"
                                  value={getDisplayMinutes(data?.cookTimeInMinutes)}/>
                        <InfoPill iconName="bar-chart-2" label="Total"
                                  value={getDisplayMinutes(data?.totalTimeInMinutes)}/>
                    </View>

                    {data?.ingredients && data.ingredients.length > 0 && (
                        <SectionCard title="Ingredients">
                            {data.ingredients.map((ingredient) => (
                                <View key={ingredient} style={tw`flex-row items-center mb-2`}>
                                    <ThemeText style={tw`flex-1`}>• {ingredient}</ThemeText>
                                </View>
                            ))}
                        </SectionCard>
                    )}

                    {data?.directions && data.directions.length > 0 && (
                        <SectionCard title="Directions">
                            {data.directions.map((direction, index) => (
                                <View key={direction} style={tw`flex-row mb-3`}>
                                    <ThemeText style={tw`mr-3`}>{index + 1}.</ThemeText>
                                    <ThemeText style={tw`flex-1`}>{direction}</ThemeText>
                                </View>
                            ))}
                        </SectionCard>
                    )}

                    {data?.notes && data.notes.length > 0 && (
                        <SectionCard title="Notes">
                            {data.notes.map((note) => (
                                <View key={note} style={tw`flex-row items-start mb-2`}>
                                    <ThemeText style={tw`flex-1`}>• {note}</ThemeText>
                                </View>
                            ))}
                        </SectionCard>
                    )}
                </View>

                <View style={tw`h-6`}/>
            </ScrollView>
        </ThemeSafeAreaView>
    );
}