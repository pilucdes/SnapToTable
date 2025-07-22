import {QueryClient, QueryClientProvider} from "@tanstack/react-query";
import {Stack} from "expo-router";
import { StatusBar } from "expo-status-bar";
import "../styles/global.css"

const queryClient = new QueryClient();
export default function RootLayout() {
    return (
        <QueryClientProvider client={queryClient}>
            <StatusBar style="dark"></StatusBar>
            <Stack>
                <Stack.Screen name="index" options={{headerShown: false}} />
            </Stack>
        </QueryClientProvider>
    );
}
