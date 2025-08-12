import {useLocalSearchParams} from "expo-router";
import {useGetRecipeById} from "@/features/recipes/hooks/useRecipe";
import {ScrollView, View} from "react-native";
import tw from "@/lib/tailwind";
import {SectionCard} from "@/features/recipes/components/SectionCard";
import {InfoPill} from "@/features/recipes/components/InfoPill";
import {ThemeAreaViewLoading, ThemeAreaView, ThemeText, AnimationEaseIn} from "@/features/common/components";
import {RecipeImage} from "@/features/recipes/components";
import {colorTheme} from "@/themes/constants/themeConstants";

export default function RecipeDetailScreen() {
    const {id} = useLocalSearchParams<{ id: string }>();
    const {data, isLoading} = useGetRecipeById(id);
    const getDisplayMinutes = (dataMinutes: number | null | undefined) => {

        if (!dataMinutes) {
            return "";
        }

        return `${dataMinutes} min`;
    }

    if (isLoading) {
        return (
            <ThemeAreaViewLoading/>
        );
    }

    return (
        <ThemeAreaView>
            <ScrollView>

                <RecipeImage
                    url={data?.url}
                    style={tw`h-64`}
                />

                <View style={tw`p-6 lg:px-100 gap-4`}>

                    <AnimationEaseIn>
                        <ThemeText variant="title" color={colorTheme.accent.opt3} style={tw`font-bold`}>
                            {data?.name}
                        </ThemeText>
                        <ThemeText variant="heading" style={tw`mt-2 px-3 py-1 rounded-full self-start font-bold`}>
                            {data?.category}
                        </ThemeText>
                    </AnimationEaseIn>

                    <AnimationEaseIn delay={50}>
                        <View style={tw`flex-row justify-around items-center`}>
                            <InfoPill iconName="users" label="Serves" value={`${data?.servings}`}/>
                            <InfoPill iconName="clock" label="Prep" value={getDisplayMinutes(data?.prepTimeInMinutes)}/>
                            <InfoPill iconName="thermometer" label="Cook"
                                      value={getDisplayMinutes(data?.cookTimeInMinutes)}/>
                            <InfoPill iconName="bar-chart-2" label="Total"
                                      value={getDisplayMinutes(data?.totalTimeInMinutes)}/>
                        </View>
                    </AnimationEaseIn>

                    {data?.ingredients && data.ingredients.length > 0 && (

                        <AnimationEaseIn delay={100}>
                            <SectionCard title="Ingredients">
                                {data.ingredients.map((ingredient) => (
                                    <View key={ingredient} style={tw`flex-row items-center mb-2`}>
                                        <ThemeText style={tw`flex-1`}>• {ingredient}</ThemeText>
                                    </View>
                                ))}
                            </SectionCard>
                        </AnimationEaseIn>
                    )}

                    {data?.directions && data.directions.length > 0 && (
                        <AnimationEaseIn delay={150}>
                            <SectionCard title="Directions">
                                {data.directions.map((direction, index) => (
                                    <View key={direction} style={tw`flex-row mb-3`}>
                                        <ThemeText style={tw`mr-3`}>{index + 1}.</ThemeText>
                                        <ThemeText style={tw`flex-1`}>{direction}</ThemeText>
                                    </View>
                                ))}
                            </SectionCard>
                        </AnimationEaseIn>
                    )}

                    {data?.notes && data.notes.length > 0 && (
                        <AnimationEaseIn delay={200}>
                            <SectionCard title="Notes">
                                {data.notes.map((note) => (
                                    <View key={note} style={tw`flex-row items-start mb-2`}>
                                        <ThemeText style={tw`flex-1`}>• {note}</ThemeText>
                                    </View>
                                ))}
                            </SectionCard>
                        </AnimationEaseIn>
                    )}
                </View>

                <View style={tw`h-6`}/>
            </ScrollView>
        </ThemeAreaView>
    );
}