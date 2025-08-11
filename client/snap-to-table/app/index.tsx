import {View} from "react-native";
import {useCreateRecipeAnalysis} from "../features/recipes/hooks/useRecipe";
import {useRecipeImagePicker} from "@/features/recipes/hooks/useRecipeImagePicker";
import tw from "@/lib/tailwind"
import {CreateRecipeAnalysisRequestDto} from "@/features/recipes/api/dto";
import {
    Icon,
    ThemeButton,
    ThemeAreaView,
    ThemeText,
    ThemeSwitcher,
    AnimationEaseIn
} from "@/features/common/components";
import {router} from "expo-router";

export default function HomeScreen() {

    const {mutate: createRecipeAnalysis, isPending} = useCreateRecipeAnalysis();
    const {snapImages} = useRecipeImagePicker();
    const handleSnapPress = async () => {

        const imageAsset = await snapImages();

        if (imageAsset && imageAsset.length > 0) {
            try {

                const imagesBlob = await Promise.all(imageAsset.map(async (asset) => {
                    const response = await fetch(asset.uri);
                    return await response.blob();
                }));

                const payload: CreateRecipeAnalysisRequestDto = {
                    images: imagesBlob
                };

                createRecipeAnalysis(payload);

            } catch (err) {
                console.error(err);
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
                    <ThemeText variant="title" style={tw`text-4xl font-semibold text-center mb-10`}>
                        {isPending ? "Creating new recipes..." : "Any new recipe needed for today ?"}
                    </ThemeText>
                </AnimationEaseIn>


                <AnimationEaseIn delay={200}>
                    <ThemeButton
                        disabled={isPending}
                        onPress={handleSnapPress}
                        isLoading={isPending}
                        style={tw`w-80`}
                    >

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
                        style={tw`mt-5 w-80`}
                    >

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