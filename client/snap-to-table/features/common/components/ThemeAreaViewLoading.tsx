import tw from "@/lib/tailwind";

import { ActivityIndicator } from "react-native";
import { ThemeAreaView } from "./ThemeAreaView";

export const ThemeAreaViewLoading = () => {

    return (
        <ThemeAreaView style={tw`items-center justify-center`}>
            <ActivityIndicator size="large"/>
        </ThemeAreaView>
    );
}