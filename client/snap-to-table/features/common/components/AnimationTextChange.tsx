import {Animated, Easing, StyleProp, TextStyle} from "react-native"
import {ThemeText} from "./ThemeText"
import {useEffect, useRef, useState} from "react";
import {shuffle} from "@/utils/array";

interface AnimationTextChangeProps {
    content: AnimationTextContent[]
    shuffleContent?: boolean
    style?: StyleProp<TextStyle>
}

interface AnimationTextContent {
    value: string,
    duration: number
}

export const AnimationTextChange = ({content, style, shuffleContent = false}: AnimationTextChangeProps) => {

    const [textToDisplay, setTextToDisplay] = useState("");
    const textOpacity = useRef(new Animated.Value(1)).current;
    const scheduleTextAnimations = () => {

        if (shuffleContent) {
            shuffle(content)
        }

        let timeoutIds = [];
        let delay = 0;
        for (let i = 0; i < content.length; i++) {

            const timeoutId = setTimeout(() => {

                Animated.timing(textOpacity, {
                    toValue: 0,
                    duration: 150,
                    useNativeDriver: true,
                    easing: Easing.ease,
                }).start(() => {
                    
                    setTextToDisplay(content[i].value);
                    
                    Animated.timing(textOpacity, {
                        toValue: 1,
                        duration: 150,
                        useNativeDriver: true,
                        easing: Easing.ease,
                    }).start();
                    
                });

            }, delay);

            timeoutIds.push(timeoutId);

            delay += content[i].duration;
        }

        return timeoutIds;
    }

    useEffect(() => {
        if (content.length <= 1) return;

        const timeoutIds = scheduleTextAnimations();
        return () => {
            timeoutIds.forEach(id => clearTimeout(id));
        }
    }, [content]);
    

    if (content.length === 0) {
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