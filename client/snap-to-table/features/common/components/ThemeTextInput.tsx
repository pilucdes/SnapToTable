import tw from "@/lib/tailwind";
import {StyleProp, TextInput, TextStyle} from "react-native"
import {applyOpacityToHex} from "@/utils/colors";
import {useTheme} from "@/themes";

interface ThemeTextInputProps {
    value: string,
    onChangeText: (value: string) => void
    placeholder?: string
    style?: StyleProp<TextStyle>
}

export const ThemeTextInput = ({value, onChangeText, placeholder, style}: ThemeTextInputProps) => {

    const {theme} = useTheme();
    const baseStyle = tw.style(`text-lg p-2 w-full rounded-lg border-2 border-[${theme.primary}] bg-[${applyOpacityToHex(theme.primary, 0.6)}] text-[${theme.text}]`, {fontFamily: theme.font.family});

    return (
        <TextInput
            style={[baseStyle, style]}
            placeholder={placeholder}
            value={value}
            onChangeText={onChangeText}
            clearButtonMode="while-editing"
        />
    );
}