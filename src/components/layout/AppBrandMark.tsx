import { Box, Typography } from '@mui/material';
import { useTranslation } from 'react-i18next';

const logoSrc = `${import.meta.env.BASE_URL}Elwark.svg`;

type Props = {
  /** Logo height in px; width follows intrinsic aspect ratio. */
  logoHeight?: number;
  titleVariant?: 'h6' | 'subtitle1';
};

export function AppBrandMark({ logoHeight = 32, titleVariant = 'h6' }: Props) {
  const { t } = useTranslation();

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, minWidth: 0 }}>
      <Box
        component="img"
        src={logoSrc}
        alt=""
        sx={{
          height: logoHeight,
          width: 'auto',
          maxWidth: logoHeight * 3.5,
          display: 'block',
          flexShrink: 1,
          minWidth: 0,
          objectFit: 'contain',
          objectPosition: 'left center',
        }}
      />
      <Typography
        variant={titleVariant}
        component="span"
        fontWeight={500}
        noWrap
        sx={{
          color: 'text.primary',
          lineHeight: 1.2,
          fontSize: titleVariant === 'h6' ? '1.3125rem' : '1.0625rem',
        }}
      >
        {t('nav.account')}
      </Typography>
    </Box>
  );
}
