import {useCallback, useState} from 'react';
import {Pressable, View} from 'react-native';
import tw from "@/lib/tailwind"
import {RecipeSummary} from '../types';
import {router} from 'expo-router';
import {Icon, ThemeButton, ThemeText} from '@/features/common/components';

interface RecipeCardProps {
    recipe: RecipeSummary;
}

export const RecipeCard = ({recipe}: RecipeCardProps) => {
    
    const handlePress = useCallback(() => {
        router.push(`/recipes/${recipe.id}`);
    }, [recipe.id])
    
    return (
        <ThemeButton onPress={handlePress}>
            <ThemeText variant="title">{recipe.name}</ThemeText>
            <ThemeText variant="heading" style={tw`mt-1`}>{recipe.category}</ThemeText>

            <View style={tw`mt-4 border-t border-gray-200 dark:border-gray-700 pt-4`}>
                {recipe.ingredients.slice(0, 5).map((ingredient) => (
                    <ThemeText variant="caption" key={ingredient}><Icon name="circle"/> {ingredient}</ThemeText>
                ))}
            </View>
        </ThemeButton>
    );
};