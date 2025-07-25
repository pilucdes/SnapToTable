import { useState } from 'react';
import { Pressable, Text, View } from 'react-native';
import tw from "@/lib/tailwind"
import Recipe from '../types/recipe';

interface RecipeCardProps {
    recipe: Recipe;
}

export const RecipeCard = ({ recipe }: RecipeCardProps) => {
    
    const [isHovered, setIsHovered] = useState(false);

    return (
        <Pressable
            onHoverIn={() => setIsHovered(true)}
            onHoverOut={() => setIsHovered(false)}
            style={tw.style(
                `w-full max-w-lg shadow-sm rounded-lg p-6`,
                `bg-white dark:bg-gray-800`,
                isHovered && `bg-gray-100 dark:bg-gray-700`
            )}
        >
            <Text style={tw`text-2xl font-bold text-zinc-900 dark:text-white`}>{recipe.name}</Text>
            <Text style={tw`text-lg mt-1 text-gray-600 dark:text-gray-300`}>{recipe.category}</Text>

            <View style={tw`mt-4 border-t border-gray-200 dark:border-gray-700 pt-4`}>
                {recipe.ingredients.slice(0, 5).map((ingredient) => (
                    <Text key={ingredient} style={tw`text-sm text-gray-500 dark:text-gray-400`}>• {ingredient}</Text>
                ))}
            </View>
        </Pressable>
    );
};