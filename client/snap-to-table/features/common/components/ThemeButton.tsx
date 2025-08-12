import tw from "@/lib/tailwind";
import {ActivityIndicator, GestureResponderEvent, Pressable, PressableProps, StyleProp, ViewStyle} from "react-native";
import {useState} from "react";
import {colorTheme} from "@/themes/constants/themeConstants";
import { applyOpacityToHex } from "@/utils/colors";

interface ThemeButtonProps extends PressableProps {
    variant?: ButtonVariant,
    onPress?: (event: GestureResponderEvent) => void;
    style?: StyleProp<ViewStyle>,
    children?: React.ReactNode,
    isLoading?: boolean
}

const commonStyle = `flex-row items-center justify-center px-8 py-4 rounded-xl`;
const variants = {
    none: ``,
    label: commonStyle,
    primary: `bg-[${colorTheme.primary}] shadow-lg ${commonStyle}`,
    subtilePrimary: `bg-[${applyOpacityToHex(colorTheme.primary,0.5)}] shadow-lg ${commonStyle}`

};

type ButtonVariant = keyof typeof variants;

export const ThemeButton = ({style, onPress, children, isLoading, variant = "primary", ...rest}: ThemeButtonProps) => {

    const buttonStyle = tw.style(variants[variant]);
    const [isPressed, setOnPressed] = useState(false);
    const baseStyle = tw.style(
        isPressed && `opacity-90`,
        rest.disabled && `opacity-50`
    );

    return (
        <Pressable
            {...rest}
            onPressIn={() => setOnPressed(true)}
            onPressOut={() => setOnPressed(false)}
            style={[baseStyle, buttonStyle, style]}
            onPress={onPress}>

            {isLoading ? (
                <ActivityIndicator size="small" color={colorTheme.secondary}/>
            ) : (
                children
            )}
        </Pressable>
    );
}