import React from 'react';
import {Text, TextProps} from 'react-native';
import tw from '@/lib/tailwind';
import { useTheme } from '@/features/themes';
import { ColorValueHex } from '@/features/themes/types';

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

    const {theme} = useTheme();
    const typographyStyle = tw.style(variantTypography[variant], {fontFamily: theme.font.family});
    
    let colorStyle;

    switch (variant) {
        case "error":
            colorStyle = tw.style(`text-[${theme.error}]`);
            break;
        default:
            colorStyle = color ? tw.style(`text-[${color}]`) : tw.style(`text-[${theme.text}]`);
    }

    return (
        <Text style={[typographyStyle, colorStyle, style]} {...rest}>
            {children}
        </Text>
    );
};