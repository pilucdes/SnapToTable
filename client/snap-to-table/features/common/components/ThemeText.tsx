import React from 'react';
import {Text, TextProps} from 'react-native';
import tw from '@/lib/tailwind';

const variantClasses = {
    title: "text-3xl font-bold tracking-tight text-zinc-900 dark:text-white",
    heading: "text-xl font-bold text-zinc-900 dark:text-white",
    subheading: "text-lg text-gray-600 dark:text-gray-300",
    body: "text-base text-gray-700 dark:text-gray-300",
    caption: "text-sm text-gray-500 dark:text-gray-400",
    subcaption: "text-xs text-gray-500 dark:text-gray-400",
    link: "text-base font-semibold text-blue-600 dark:text-blue-400",
    error: "text-sm text-red-600 dark:text-red-500",
};

type TextVariant = keyof typeof variantClasses;

interface ThemeTextProps extends TextProps {
    children: React.ReactNode;
    variant?: TextVariant;
}

export const ThemeText = ({
                              children,
                              variant = 'body',
                              style,
                              ...rest
                          }: ThemeTextProps) => {

    const classString = variantClasses[variant];
    const baseStyle = tw.style(classString);

    return (
        <Text style={[baseStyle, style]} {...rest}>
            {children}
        </Text>
    );
};