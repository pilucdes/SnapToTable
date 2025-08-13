import {RecipeCard} from "@/features/recipes/components/RecipeCard";
import {useGetAllRecipes} from "@/features/recipes/hooks/useRecipe";
import {useLocalSearchParams} from "expo-router";
import {ActivityIndicator, FlatList} from "react-native";
import tw from "@/lib/tailwind";
import {ThemeAreaViewLoading, ThemeMessage, ThemeAreaView, AnimationEaseIn} from "@/features/common/components";
import {useCallback, useState} from "react";
import {RecipeSummary} from "@/features/recipes/types";
import {useDebounce} from "@/features/common/hooks/useDebounce";
import {ThemeTextInput} from "@/features/common/components";

const RECIPE_PAGE_SIZE = 5;
const RECIPE_SEARCH_DELAY = 300;

export default function RecipesScreen() {

    const {recipeAnalysisId} = useLocalSearchParams<{ recipeAnalysisId: string }>();
    const [searchFilter, setSearchFilter] = useState("");
    const allowSearchFilter = !recipeAnalysisId;
    const debounceText = useDebounce(searchFilter, RECIPE_SEARCH_DELAY);

    const {
        data,
        error,
        isFetchingNextPage,
        hasNextPage,
        fetchNextPage,
        refetch,
        isLoading,
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

    const renderItem = useCallback(({item, index}: { item: RecipeSummary, index: number }) => (
        <AnimationEaseIn delay={Math.min(index * 50, 300)}>
            <RecipeCard recipe={item}/>
        </AnimationEaseIn>
    ), []);

    if (isLoading) {
        return (<ThemeAreaViewLoading/>)
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
                data={recipes}
                renderItem={renderItem}
                keyExtractor={(item, index) => `${item.id}-${index}`}
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