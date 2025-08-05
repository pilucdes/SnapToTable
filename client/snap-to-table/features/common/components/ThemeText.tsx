import React from 'react';
import {Text, TextProps} from 'react-native';
import tw from '@/lib/tailwind';
import {lightTheme, darkTheme, ColorValueHex} from "../themes";

const variantTypography = {
    title: `text-3xl`,
    heading: `text-xl`,
    subheading: `text-lg`,
    body: `text-base`,
    caption: `text-sm`,
    subcaption: `text-xs`,
    link: `text-base`,
    error: `text-sm`
};

type TextVariant = keyof typeof variantTypography;

interface ThemeTextProps extends TextProps {
    children: React.ReactNode;
    variant?: TextVariant;
    color?: ColorValueHex;
}

export const ThemeText = ({
                              children,
                              variant = 'body',
                              style,
                              color,
                              ...rest
                          }: ThemeTextProps) => {

    const typographyStyle = tw.style(variantTypography[variant], {fontFamily: 'Poppins_400Regular'});

    let colorStyle;

    switch (variant) {
        case "error":
            colorStyle = tw.style(`text-[${lightTheme.error}] dark:text-[${darkTheme.error}]`);
            break;
        default:
            colorStyle = color ? tw.style(`text-[${color}]`) : tw.style(`text-[${lightTheme.text}] dark:text-[${darkTheme.text}]`);
    }

    return (
        <Text style={[typographyStyle, colorStyle, style]} {...rest}>
            {children}
        </Text>
    );
};