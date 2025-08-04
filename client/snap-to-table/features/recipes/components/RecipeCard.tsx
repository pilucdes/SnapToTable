import {useCallback, useState} from 'react';
import {View, Image} from 'react-native';
import tw from "@/lib/tailwind"
import {RecipeSummary} from '../types';
import {router} from 'expo-router';
import {ThemeButton, ThemeText} from '@/features/common/components';
import {colorTheme} from "@/features/common/themes";

interface RecipeCardProps {
    recipe: RecipeSummary;
}

export const RecipeCard = ({recipe}: RecipeCardProps) => {

    const handlePress = () => {
        router.push(`/recipes/${recipe.id}`);
    }

    return (
        <ThemeButton variant="label" onPress={handlePress}>
            <View style={tw`w-80 md:w-180 bg-[${colorTheme.primary}] rounded-xl shadow-lg`}>

                <Image
                    source={{
                        uri: recipe.url || require('@/assets/images/default-recipe.webp')
                    }}
                    style={tw`h-32 md:h-40 rounded-t-xl` }
                    resizeMode="cover"
                />
                
                <View style={tw`p-6`}>
                    <ThemeText variant="title">{recipe.name}</ThemeText>
                    <ThemeText variant="heading" style={tw`mt-1`}>{recipe.category}</ThemeText>

                    <View style={tw`mt-4`}>
                        <ThemeText variant="subheading" style={tw`my-1`}>Ingredients</ThemeText>
                        {recipe.ingredients.map((ingredient) => (
                            <ThemeText style={tw`flex gap-1`} variant="caption"
                                       key={ingredient}>• {ingredient}</ThemeText>
                        ))}
                    </View>
                </View>

            </View>

        </ThemeButton>
    );
};