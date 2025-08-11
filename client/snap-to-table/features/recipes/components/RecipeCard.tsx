import {View} from 'react-native';
import tw from "@/lib/tailwind"
import {RecipeSummary} from '../types';
import {router} from 'expo-router';
import {ThemeButton, ThemeText} from '@/features/common/components';
import {LinearGradient} from 'expo-linear-gradient';
import {applyOpacityToHex} from '@/utils/colors';
import {colorTheme} from '@/features/themes/constants/themeConstants';
import {RecipeImage} from './RecipeImage';

interface RecipeCardProps {
    recipe: RecipeSummary;
}

export const RecipeCard = ({recipe}: RecipeCardProps) => {

    const handlePress = () => {
        router.push(`/recipes/${recipe.id}`);
    }

    return (
        <ThemeButton variant="none" onPress={handlePress}>
            <LinearGradient
                colors={[colorTheme.primary, applyOpacityToHex(colorTheme.primary, 0.80)]}
                start={{x: 0, y: 0}}
                end={{x: 0.2, y: 1}}
                style={tw`w-80 md:w-180 rounded-xl shadow-lg overflow-hidden md:flex-row`}
            >

                <RecipeImage url={recipe.url}
                             style={tw`h-32 md:h-full md:w-1/3 rounded-t-xl md:rounded-l-xl md:rounded-tr-none`}
                             resizeMode="cover"
                />

                <View style={tw`p-6 md:w-2/3`}>
                    <ThemeText variant="title" color={colorTheme.accent.opt3}
                               style={tw`font-bold`}>{recipe.name}</ThemeText>
                    <ThemeText variant="heading" style={tw`mt-1 text-white`}>{recipe.category}</ThemeText>

                    <View style={tw`mt-4`}>
                        <ThemeText variant="subheading" style={tw`my-1 text-white`}>Ingredients</ThemeText>
                        {recipe.ingredients.map((ingredient) => (
                            <ThemeText style={tw`flex gap-1 text-white`} variant="caption"
                                       key={ingredient}>• {ingredient}</ThemeText>
                        ))}
                    </View>
                </View>
            </LinearGradient>
        </ThemeButton>
    );
};