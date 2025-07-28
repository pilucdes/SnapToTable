import {Text, View, ActivityIndicator, SafeAreaView} from "react-native";
import {Feather} from '@expo/vector-icons';
import {useCreateRecipeAnalysis} from "../features/recipes/hooks/useRecipe";
import {useRecipeImagePicker} from "@/features/recipes/hooks/useRecipeImagePicker";
import tw from "@/lib/tailwind"
import {CreateRecipeAnalysisRequestDto} from "@/features/recipes/api/dto";
import {ThemeButton, ThemeSafeAreaView, ThemeText} from "@/features/common/components";

export default function HomeScreen() {

    const {mutate: createRecipeAnalysis, error, isPending} = useCreateRecipeAnalysis();
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

    return (
        <ThemeSafeAreaView>

            <View style={tw`flex-1 items-center justify-center p-8`}>
                <ThemeText variant="title" style={tw`text-4xl font-semibold text-center mb-10`}>
                    {isPending ? "Creating new recipes..." : "Any new recipe needed for today ?"}
                </ThemeText>

                <ThemeButton
                    disabled={isPending}
                    onPress={handleSnapPress}
                    style={tw.style(
                        `flex-row items-center justify-center px-8 py-4 rounded-xl shadow-lg`
                    )}
                >
                    {isPending ? (
                        <ActivityIndicator size="small" color="white"/>
                    ) : (
                        <>
                            <Feather name="camera" size={24} style={tw`mr-3 text-white`}/>
                            <Text style={tw`text-lg font-bold text-white`}>
                                Snap a recipe
                            </Text>
                        </>
                    )}
                </ThemeButton>

                {error && (
                    <ThemeText variant="error" style={tw`mt-6 text-center`}>
                        {error.message}
                    </ThemeText>
                )}
            </View>
        </ThemeSafeAreaView>
    );
}