import {Animated, Easing, StyleProp, TextStyle} from "react-native"
import {ThemeText} from "./ThemeText"
import {useEffect, useRef, useState} from "react";
import {shuffle} from "@/utils/array";

interface AnimationTextChangeProps {
    textContent: AnimationTextContent[]
    shuffleContent?: boolean
    style?: StyleProp<TextStyle>
}

interface AnimationTextContent {
    value: string,
    duration: number
}

export const AnimationTextChange = ({textContent, style, shuffleContent = false}: AnimationTextChangeProps) => {

    const [textToDisplay, setTextToDisplay] = useState("");
    const textOpacity = useRef(new Animated.Value(1)).current;
    const scheduleTextAnimations = () => {

        if (shuffleContent) {
            shuffle(textContent)
        }

        let timeoutIds = [];
        let delay = 0;
        
        for (const content of textContent) {

            const timeoutId = setTimeout(() => {

                Animated.timing(textOpacity, {
                    toValue: 0,
                    duration: 150,
                    useNativeDriver: true,
                    easing: Easing.ease,
                }).start(() => {

                    setTextToDisplay(content.value);

                    Animated.timing(textOpacity, {
                        toValue: 1,
                        duration: 150,
                        useNativeDriver: true,
                        easing: Easing.ease,
                    }).start();

                });

            }, delay);

            timeoutIds.push(timeoutId);

            delay += content.duration;
        }

        return timeoutIds;
    }

    useEffect(() => {
        if (textContent.length <= 1) return;

        const timeoutIds = scheduleTextAnimations();
        return () => {
            timeoutIds.forEach(id => clearTimeout(id));
        }
    }, [textContent]);


    if (textContent.length === 0) {
        return null;
    }

    return (
        <Animated.View style={{
            opacity: textOpacity,
        }}>
            <ThemeText style={[style]}>{textToDisplay}</ThemeText>
        </Animated.View>
    )

}