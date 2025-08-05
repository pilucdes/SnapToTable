import { FALLBACK_RECIPE_IMAGE } from '@/features/common/constants/images';
import { darkTheme, lightTheme } from '@/features/common/themes';
import tw from '@/lib/tailwind';
import React, {useRef, useState} from 'react';
import {
    Image,
    ImageSourcePropType,
    StyleProp,
    ImageStyle,
    Animated,
    StyleSheet,
    View,
    ImageResizeMode,
} from 'react-native';



interface RecipeImageProps {
    url?: string | null;
    style?: StyleProp<ImageStyle>;
    resizeMode?: ImageResizeMode;
}

export const RecipeImage = ({url, style, resizeMode = 'cover'}: RecipeImageProps) => {
   
    const imageOpacity = useRef(new Animated.Value(0)).current;

    const [imageSource, setImageSource] = useState<ImageSourcePropType>(
        url ? {uri: url} : FALLBACK_RECIPE_IMAGE
    );

    const onImageLoad = () => {
        Animated.timing(imageOpacity, {
            toValue: 1,
            duration: 400,
            useNativeDriver: true,
        }).start();
    };
    
    const handleImageError = () => {
        setImageSource(FALLBACK_RECIPE_IMAGE);
    };
    
    const containerStyle = tw.style(`w-full bg-[${lightTheme.background}] dark:bg-[${darkTheme.background}]`);
    
    return (
        <View style={[containerStyle, style]}>

            <Image
                source={imageSource}
                style={[StyleSheet.absoluteFill, {width: '100%', height: '100%'}]}
                resizeMode={resizeMode}
                blurRadius={10}
            />
            
            <Animated.Image
                source={imageSource}
                style={[
                    StyleSheet.absoluteFill,
                    {width: '100%', height: '100%'},
                    {opacity: imageOpacity},
                ]}
                resizeMode={resizeMode}
                onLoad={onImageLoad}
                onError={handleImageError}
            />
        </View>
    );
};