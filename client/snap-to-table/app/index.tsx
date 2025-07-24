import {Text, View, Pressable, ActivityIndicator, SafeAreaView} from "react-native";
import {Camera} from "lucide-react-native";
import {useCreateRecipeAnalysis} from "../features/recipes/hooks/useRecipe";
import {useRecipeImagePicker} from "@/features/recipes/hooks/useRecipeImagePicker";
import CreateRecipeAnalysisRequestDto from "@/features/recipes/api/dto/createRecipeAnalysisRequestDto";
import tw from "@/lib/tailwind"

export default function IndexScreen() {

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
        <SafeAreaView style={tw`flex-1 dark:bg-zinc-900`}>

            <View style={tw`flex-1 items-center justify-center p-8`}>
                <Text style={tw`text-4xl font-semibold text-center mb-10 dark:text-white`}>
                    {isPending ? "Creating new recipes..." : "Any new recipe needed for today ?"}
                </Text>

                <Pressable
                    disabled={isPending}
                    onPress={handleSnapPress}
                    style={({pressed}) => tw.style(
                        `flex-row items-center justify-center px-8 py-4 rounded-xl shadow-lg`,
                        pressed ? `bg-indigo-700` : `bg-indigo-600`,
                        isPending && `opacity-50`
                    )}
                >
                    {isPending ? (
                        <ActivityIndicator size="small" color="white"/>
                    ) : (
                        <>
                            <Camera size={24} style={tw`mr-3 text-white`}/>
                            <Text style={tw`text-lg font-bold text-white`}>
                                Snap a recipe
                            </Text>
                        </>
                    )}
                </Pressable>

                {error && (
                    <Text style={tw`text-red-400 mt-6 text-center`}>
                        {error.message}
                    </Text>
                )}
            </View>
        </SafeAreaView>
    );
}