import tw from "@/lib/tailwind";
import {ThemeSafeAreaView, ThemeText} from ".";

interface EmptyStateProps {
    isLoading: boolean | null;
    title: string;
    message: string | null;
    error: Error | null;
}

export const EmptyMessageState = ({isLoading, title, message, error}: EmptyStateProps) => {

    if (isLoading) {
        return null;
    }

    if (error) {
        return (
            <ThemeSafeAreaView style={tw`items-center justify-center`}>
                <ThemeText variant="title">Something went wrong.</ThemeText>
                <ThemeText variant="title">😥</ThemeText>
            </ThemeSafeAreaView>
        )
    }

    return (
        <ThemeSafeAreaView style={tw`items-center justify-center`}>
            <ThemeText variant="title">{title}</ThemeText>
            {message && (
                <ThemeText variant="title">
                    {message}
                </ThemeText>
            )}
        </ThemeSafeAreaView>
    );
}