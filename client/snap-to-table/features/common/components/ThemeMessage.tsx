import tw from "@/lib/tailwind";
import { ThemeAreaView } from "./ThemeAreaView";
import { ThemeText } from "./ThemeText";

interface ThemeMessageProps {
    isLoading?: boolean | null;
    title?: string;
    message?: string | null;
    error?: Error | null;
}

export const ThemeMessage = ({isLoading, title, message, error}: ThemeMessageProps) => {

    if (isLoading) {
        return null;
    }

    if (error) {
        return (
            <ThemeAreaView style={tw`items-center justify-center gap-2`}>
                <ThemeText variant="title">Something went wrong.</ThemeText>
                <ThemeText variant="heading">{error.message}</ThemeText>
                <ThemeText variant="heading">😥</ThemeText>
            </ThemeAreaView>
        )
    }

    return (
        <ThemeAreaView style={tw`items-center justify-center gap-2`}>
            <ThemeText variant="title">{title}</ThemeText>
            {message && (
                <ThemeText variant="title">
                    {message}
                </ThemeText>
            )}
        </ThemeAreaView>
    );
}