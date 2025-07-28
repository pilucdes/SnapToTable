import { ThemeText } from "@/features/common/components/ThemeText";
import tw from "@/lib/tailwind";
import {View} from "react-native";

interface SectionCardProps {
    title: string,
    children?: React.ReactNode
}

export const SectionCard = ({title, children} : SectionCardProps) => (
    <View style={tw`w-full bg-white dark:bg-zinc-800 p-4 rounded-xl shadow-sm mt-6`}>
        <ThemeText variant="heading" style={tw`mb-3`}>{title}</ThemeText>
        {children}
    </View>
);