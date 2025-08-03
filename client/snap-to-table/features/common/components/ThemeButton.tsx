import tw from "@/lib/tailwind";
import {GestureResponderEvent, Pressable, PressableProps, StyleProp, ViewStyle} from "react-native";
import {useState} from "react";
import {colorTheme} from "../themes"

interface ThemeButtonProps extends PressableProps {
    variant?: ButtonVariant,
    onPress?: (event: GestureResponderEvent) => void;
    style?: StyleProp<ViewStyle>,
    children?: React.ReactNode,
}

const variants = {
    label: ``,
    styled: `bg-[${colorTheme.primary}] shadow-sm p-6`
};

type ButtonVariant = keyof typeof variants;

export const ThemeButton = ({style, onPress, children,variant = "styled", ...rest}: ThemeButtonProps) => {

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
            style={[baseStyle,buttonStyle, style]}
            onPress={onPress}>
            {children}
        </Pressable>
    );
}