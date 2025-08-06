import tw from "@/lib/tailwind";
import {StyleProp, TextInput, TextStyle} from "react-native"
import {darkTheme, fontTheme, lightTheme} from "../themes";
import {applyOpacityToHex} from "@/utils/colors";

interface ThemeTextInputProps {
    value: string,
    onChangeText: (value: string) => void
    placeholder?: string
    style?: StyleProp<TextStyle>
}

export const ThemeTextInput = ({value, onChangeText, placeholder, style}: ThemeTextInputProps) => {

    const baseStyle = tw.style(`text-lg p-2 w-full rounded-lg border-2 border-[${darkTheme.primary}] bg-[${applyOpacityToHex(darkTheme.primary, 0.5)}] text-[${lightTheme.text}] dark:text-[${darkTheme.text}]`, {fontFamily: fontTheme.family});

    return (
        <TextInput
            style={[baseStyle, style]}
            placeholder={placeholder}
            value={value}
            onChangeText={onChangeText}
            clearButtonMode="while-editing"
            autoFocus={true}
        />
    );
}