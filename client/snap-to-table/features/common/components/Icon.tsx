import {IconName} from "@/features/recipes/types"
import tw from "@/lib/tailwind"
import {Feather} from "@expo/vector-icons"
import {StyleProp, TextStyle} from "react-native"

interface IconProps {
    name: IconName,
    size?: number,
    style?: StyleProp<TextStyle>
}

export const Icon = ({name, size = 24, style}: IconProps) => {

    const baseStyle = tw`text-gray-700 dark:text-gray-300`;

    return (<Feather name={name} size={size} style={[baseStyle, style]}></Feather>)
}