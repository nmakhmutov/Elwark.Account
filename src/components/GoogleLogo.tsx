import type { ImgHTMLAttributes } from 'react';
import { brandImageSrc } from './brandImageSrc';

type Props = Omit<ImgHTMLAttributes<HTMLImageElement>, 'src' | 'width' | 'height' | 'alt'> & {
  size?: number;
};

export function GoogleLogo({ size = 28, style, ...rest }: Props) {
  return (
    <img
      src={brandImageSrc('google')}
      width={size}
      height={size}
      alt=""
      aria-hidden
      decoding="async"
      draggable={false}
      style={{ display: 'block', flexShrink: 0, objectFit: 'contain', ...style }}
      {...rest}
    />
  );
}
