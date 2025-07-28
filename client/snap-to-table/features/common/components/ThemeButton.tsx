import tw from "@/lib/tailwind";
import {GestureResponderEvent, Pressable, PressableProps, StyleProp, ViewStyle} from "react-native";
import {useState} from 'react';

interface ThemeButtonProps extends PressableProps {
    onPress?: (event: GestureResponderEvent) => void;
    style?: StyleProp<ViewStyle>,
    children?: React.ReactNode,
}

export const ThemeButton = ({style, onPress, children, ...rest}: ThemeButtonProps) => {

    const [isHovered, setIsHovered] = useState(false);
    const baseStyle = tw.style(
        `w-full max-w-lg shadow-sm rounded-lg p-6`,
        `bg-white dark:bg-gray-800`,
        isHovered && `bg-gray-100 dark:bg-gray-700`,
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