import React from 'react';
import {Text, TextProps} from 'react-native';
import tw from '@/lib/tailwind';
import {lightTheme, darkTheme} from "../themes";

const variantTypography = {
    title: `text-3xl font-bold`,
    heading: `text-xl font-bold`,
    subheading: `text-lg `,
    body: `text-base`,
    caption: `text-sm`,
    subcaption: `text-xs`,
    link: `text-base font-bold`,
    error: `text-sm`,
};

type TextVariant = keyof typeof variantTypography;

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

    const typographyStyle = tw.style(variantTypography[variant]);

    let colorStyle;

    switch (variant) {
        case "error":
            colorStyle = tw.style(`text-[${lightTheme.error}] dark:text-[${darkTheme.error}]`);
        default:
            colorStyle = tw.style(`text-[${lightTheme.text}] dark:text-[${darkTheme.text}]`);
    }

    return (
        <Text style={[typographyStyle, colorStyle, style]} {...rest}>
            {children}
        </Text>
    );
};