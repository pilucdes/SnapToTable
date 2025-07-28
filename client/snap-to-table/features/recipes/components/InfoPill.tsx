import {View} from "react-native";
import tw from "@/lib/tailwind";
import {IconName} from "../types";
import { Icon, ThemeText } from "@/features/common/components";

interface InfoPillProps {
    iconName: IconName;
    label: string;
    value?: string;
}

export const InfoPill = ({iconName, label, value}: InfoPillProps) => (
    <View style={tw`flex-col items-center gap-1`}>
        <Icon name={iconName}/>
        <ThemeText variant="caption" style={tw`font-semibold`}>{label}</ThemeText>
        <ThemeText variant="subcaption">{value}</ThemeText>
    </View>
);