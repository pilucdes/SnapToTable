import {RecipeCard} from "@/features/recipes/components/RecipeCard";
import {useGetAllRecipes} from "@/features/recipes/hooks/useRecipe";
import {useLocalSearchParams} from "expo-router";
import {ActivityIndicator, FlatList} from "react-native";
import tw from "@/lib/tailwind";
import {ThemeAreaViewLoading, ThemeMessage, ThemeAreaView} from "@/features/common/components";
import {useCallback, useState} from "react";
import {RecipeSummary} from "@/features/recipes/types";
import {useDebounce} from "@/features/common/hooks/useDebounce";
import {ThemeTextInput} from "@/features/common/components/ThemeTextInput";

const RECIPE_PAGE_SIZE = 5;
export default function RecipesScreen() {

    const {recipeAnalysisId} = useLocalSearchParams<{ recipeAnalysisId: string }>();
    const allowSearchFilter = !recipeAnalysisId;
    const [searchFilter, setSearchFilter] = useState("");
    const debounceText = useDebounce(searchFilter, 300);

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
        filter: debounceText,
        pageSize: RECIPE_PAGE_SIZE
    });

    const recipes = data?.pages.flatMap(page => page.items) ?? [];

    const listContainerStyles = tw.style(
        `items-center gap-8 pt-8 px-4 pb-8`,
        recipes.length === 0 && `flex-1 justify-center`
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
            <ThemeAreaViewLoading/>
        );
    }

    return (
        <ThemeAreaView style={tw`flex items-center justify-center`}>

            {
                allowSearchFilter && (
                    <ThemeTextInput
                        style={tw`m-4 md:w-100`}
                        placeholder="Search for a recipe..."
                        value={searchFilter}
                        onChangeText={setSearchFilter}
                    />
                )
            }

            <FlatList
                key={debounceText}
                data={recipes}
                renderItem={renderItem}
                keyExtractor={(item) => item.id}
                contentContainerStyle={listContainerStyles}
                onEndReached={handleLoadMore}
                onEndReachedThreshold={0.5}
                ListFooterComponent={renderFooter}
                onRefresh={refetch}
                refreshing={isRefetching && !isFetchingNextPage}
                ListEmptyComponent={
                    <ThemeMessage title="No recipes found." message="😳" isLoading={isRefetching} error={error}/>
                }
            />

        </ThemeAreaView>
    );
}