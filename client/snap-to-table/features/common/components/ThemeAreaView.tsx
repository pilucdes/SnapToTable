import tw from "@/lib/tailwind"
import {darkTheme, lightTheme} from "../themes"
import {SafeAreaView, SafeAreaViewProps} from "react-native-safe-area-context";

interface ThemeSafeAreaViewProps extends SafeAreaViewProps {
    children?: React.ReactNode
}

export const ThemeAreaView = ({children, style, ...rest}: ThemeSafeAreaViewProps) => {

    const baseStyle = tw`flex-1 bg-[${lightTheme.background}] dark:bg-[${darkTheme.background}]`;

    return (
        <SafeAreaView style={[baseStyle, style]} {...rest}>
            {children}
        </SafeAreaView>
    );
}