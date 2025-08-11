import {ThemeText} from "@/features/common/components/ThemeText";
import tw from "@/lib/tailwind";
import {StyleProp, View, ViewStyle} from "react-native";

interface SectionCardProps {
    title: string,
    children?: React.ReactNode,
    style?: StyleProp<ViewStyle>,
}

export const SectionCard = ({title, children, style}: SectionCardProps) => {

    const baseStyle = tw.style(`p-4 rounded-xl shadow-sm`);

    return (
        <View style={[baseStyle, style]}>
            {title ? <ThemeText variant="subheading" style={tw`mb-3 font-bold`}>{title}</ThemeText> : null}
            {children}
        </View>
    );
}