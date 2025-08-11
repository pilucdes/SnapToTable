import {useTheme} from "@/features/themes";
import tw from "@/lib/tailwind"
import {SafeAreaView, SafeAreaViewProps} from "react-native-safe-area-context";

interface ThemeSafeAreaViewProps extends SafeAreaViewProps {
    children?: React.ReactNode
}

export const ThemeAreaView = ({children, style, ...rest}: ThemeSafeAreaViewProps) => {

    const {theme} = useTheme();
    const baseStyle = tw`flex-1 bg-[${theme.background}]`;

    return (
        <SafeAreaView style={[baseStyle, style]} {...rest}>
            {children}
        </SafeAreaView>
    );
}