import {View} from "react-native";
import tw from "@/lib/tailwind"
import {recipeCreationTitles, recipeCreationIcons} from "@/features/recipes/constants/animationConstants";
import {useCreateRecipeAnalysis} from "../features/recipes/hooks/useRecipe";
import {useRecipeImagePicker} from "@/features/recipes/hooks/useRecipeImagePicker";
import {
    Icon,
    ThemeButton,
    ThemeAreaView,
    ThemeText,
    ThemeSwitcher,
    AnimationEaseIn,
    AnimationTextChange
} from "@/features/common/components";
import {router} from "expo-router";


export default function HomeScreen() {

    const {mutate: createRecipeAnalysis, isPending} = useCreateRecipeAnalysis();
    const {snapImages} = useRecipeImagePicker();
    const handleSnapPress = async () => {

        const imageAssets = await snapImages();

        if (imageAssets && imageAssets.length > 0) {
            if (imageAssets.length > 0) {
                createRecipeAnalysis(imageAssets);
            }
        }
    }

    const previousRecipesPress = () => {
        router.push(`/recipes`);
    }

    return (
        <ThemeAreaView>

            <View style={tw`items-end`}>
                <ThemeSwitcher/>
            </View>

            <View style={tw`flex-1 items-center justify-center p-8`}>
                <AnimationEaseIn delay={100}>
                    <View style={tw`mb-8 gap-3`}>
                        {isPending ?
                            <>
                                <AnimationTextChange style={tw`text-4xl text-center leading-tight`}
                                                     textContent={recipeCreationTitles}/>
                                <AnimationTextChange style={tw`text-5xl text-center leading-tight`}
                                                     textContent={recipeCreationIcons} shuffleContent={true}/>
                            </> :
                            <ThemeText variant="title" style={tw`text-4xl text-center`}>
                                Any new recipe needed for today ?
                            </ThemeText>}
                    </View>
                </AnimationEaseIn>

                <AnimationEaseIn delay={200}>
                    <ThemeButton
                        disabled={isPending}
                        onPress={handleSnapPress}
                        isLoading={isPending}
                        style={tw`w-80`}>

                        <Icon name="camera" size={24} style={tw`mr-3`}/>
                        <ThemeText style={tw`text-white`}>
                            Snap a recipe
                        </ThemeText>
                    </ThemeButton>
                </AnimationEaseIn>

                <AnimationEaseIn delay={300}>
                    <ThemeButton
                        variant="subtilePrimary"
                        onPress={previousRecipesPress}
                        style={tw`mt-5 w-80`}>

                        <Icon variant="primary" name="eye" size={24} style={tw`mr-3`}/>
                        <ThemeText style={tw`text-white`}>
                            Previous recipes
                        </ThemeText>
                    </ThemeButton>
                </AnimationEaseIn>
            </View>
        </ThemeAreaView>
    );
}