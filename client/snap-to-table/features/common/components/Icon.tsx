import {IconName} from "@/features/recipes/types"
import tw from "@/lib/tailwind"
import {Feather} from "@expo/vector-icons"
import {StyleProp, TextStyle} from "react-native"
import {colorTheme} from "../themes";

type IconVariant = 'primary' | 'secondary' | 'accent';


interface IconProps {
    name: IconName,
    size?: number,
    style?: StyleProp<TextStyle>,
    variant?: IconVariant
}

export const Icon = ({name, size = 24, variant = "primary", style}: IconProps) => {

    let baseStyle = tw`text-[${colorTheme.secondary}]`;

    switch (variant) {
        case "secondary":
            baseStyle = tw`text-[${colorTheme.secondary}]`;
            break;
        case "accent":
            baseStyle = tw`text-[${colorTheme.accent.opt1}]`;
            break;
        default:
            baseStyle = tw`text-[${colorTheme.secondary}]`;
            break;

    }

    return (<Feather name={name} size={size} style={[baseStyle, style]}></Feather>)
}