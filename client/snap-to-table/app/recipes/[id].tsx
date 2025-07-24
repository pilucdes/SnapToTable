import {useLocalSearchParams} from "expo-router";

export default function RecipeDetailScreen() {

    const {id} = useLocalSearchParams<{ id: string }>();

}