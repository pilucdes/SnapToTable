import {RecipeCard} from "@/features/recipes/components/RecipeCard";
import {useGetAllRecipes} from "@/features/recipes/hooks/useRecipe";
import {useLocalSearchParams} from "expo-router";
import {ActivityIndicator, FlatList} from "react-native";
import tw from "@/lib/tailwind";
import {EmptyMessageState, ThemeSafeAreaView} from "@/features/common/components";
import {useCallback} from "react";
import {RecipeSummary} from "@/features/recipes/types";

const RECIPE_PAGE_SIZE = 5;
export default function RecipesScreen() {

    const {recipeAnalysisId} = useLocalSearchParams<{ recipeAnalysisId: string }>();

    const {
        data,
        error,
        isLoading,
        isFetchingNextPage,
        hasNextPage,
        fetchNextPage,
        refetch,
        isRefetching
    } = useGetAllRecipes({
        recipeAnalysisId,
        filter: "",
        pageSize: RECIPE_PAGE_SIZE
    });

    const recipes = data?.pages.flatMap(page => page.items) ?? [];

    const listContainerStyles = tw.style(
        `items-center gap-8 pt-8 px-4 pb-8`,
        recipes.length === 0 && `flex-grow justify-center`
    );
    
    const renderFooter = () => {
        if (!isFetchingNextPage) return null;
        return <ActivityIndicator style={tw`my-4`}/>;
    };

    const handleLoadMore = useCallback(() => {
        if (hasNextPage && !isFetchingNextPage) {
            fetchNextPage();
        }
    }, [hasNextPage, isFetchingNextPage, fetchNextPage]);

    const renderItem = useCallback(({item}: { item: RecipeSummary }) => (
        <RecipeCard recipe={item}/>
    ), []);

    if (isLoading) {
        return (
            <ThemeSafeAreaView style={tw`items-center justify-center`}>
                <ActivityIndicator size="large"/>
            </ThemeSafeAreaView>
        );
    }

    return (
        <ThemeSafeAreaView>
            <FlatList
                data={recipes}
                renderItem={renderItem}
                keyExtractor={(item) => item.id}
                contentContainerStyle={listContainerStyles}
                onEndReached={handleLoadMore}
                onEndReachedThreshold={0.5}
                ListFooterComponent={renderFooter}
                onRefresh={refetch}
                refreshing={isRefetching}
                ListEmptyComponent={
                <EmptyMessageState title="No recipes found." message="😳" isLoading={isRefetching} error={error} />
                }
            />
        </ThemeSafeAreaView>
    );
}