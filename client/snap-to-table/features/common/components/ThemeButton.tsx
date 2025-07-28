import tw from "@/lib/tailwind";
import {GestureResponderEvent, Pressable, PressableProps, StyleProp, ViewStyle} from "react-native";
import {useState} from "react";
import {colorTheme} from "../themes"

interface ThemeButtonProps extends PressableProps {
    onPress?: (event: GestureResponderEvent) => void;
    style?: StyleProp<ViewStyle>,
    children?: React.ReactNode,
}

export const ThemeButton = ({style, onPress, children, ...rest}: ThemeButtonProps) => {

    const [isHovered, setIsHovered] = useState(false);
    const baseStyle = tw.style(
        `shadow-sm p-6`,
        `bg-[${colorTheme.secondary}]`,
        isHovered && `opacity-90`,
        rest.disabled && `opacity-50`
    );
    
    return (
        <Pressable
            {...rest}
            onHoverIn={() => setIsHovered(true)}
            onHoverOut={() => setIsHovered(false)}
            style={[baseStyle, style]}
            onPress={onPress}>
            {children}
        </Pressable>
    );
}