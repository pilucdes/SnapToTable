// app/index.tsx
import {Text, View, Pressable, ActivityIndicator, SafeAreaView} from 'react-native';
import {Camera} from 'lucide-react-native';
import {usePostRecipeAnalysis} from '../features/recipes/hooks/useRecipe';

export default function Index() {
    const {error, isPending} = usePostRecipeAnalysis();

    return (
        <SafeAreaView className="flex-1 bg-zinc-900">
            <View className="flex-1 items-center justify-center p-8">
                {/* Main Message */}
                <Text className="text-4xl font-semibold text-gray-200 text-center mb-10 animate-fadeIn">
                    Any new recipe for today?
                </Text>

                {/* Action Button */}
                <Pressable
                    className="flex-row items-center justify-center bg-indigo-600 px-8 py-4 rounded-xl shadow-lg
                     active:bg-indigo-700
                     disabled:opacity-50
                     animate-fadeIn-delay-150"
                >
                    {isPending ? (
                        <ActivityIndicator size="small" color="white"/>
                    ) : (
                        <>
                            <Camera color="white" size={24} className="mr-3"/>
                            <Text className="text-lg font-bold text-white">
                                Snap a recipe
                            </Text>
                        </>
                    )}
                </Pressable>

                {/* Error Message */}
                {error && (
                    <Text className="text-red-400 mt-6 text-center">
                        {error.message}
                    </Text>
                )}
            </View>
        </SafeAreaView>
    );
}