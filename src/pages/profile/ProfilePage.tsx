import { useState, type FormEvent } from 'react';
import {
  Box,
  Card,
  CardContent,
  TextField,
  MenuItem,
  FormControlLabel,
  Checkbox,
  Typography,
  Autocomplete,
  CircularProgress,
  createFilterOptions,
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import { useAccount, useUpdateAccount } from '../../api/hooks/useAccount';
import { useCountries } from '../../api/hooks/useWorld';
import { useTimezones } from '../../api/hooks/useWorld';
import { LoadingScreen } from '../../components/LoadingScreen';
import { LoadingButton } from '../../components/LoadingButton';
import { useSnackbar } from '../../components/SnackbarProvider';
import { ErrorScreen } from '../../components/ErrorScreen';
import { apiErrorSnackbarPayload, formatApiError } from '../../api/apiError';
import type { Account, Country, DayOfWeek, Timezone, UpdateRequest } from '../../api/types';
import { getCountryFlag, formatTimezone } from '../../api/types';
import {
  DATE_FORMATS,
  TIME_FORMATS,
  DAYS_OF_WEEK,
  formatDateExample,
} from '../../staticData';

const LANGUAGES: Record<string, string> = { en: 'English', ru: 'Русский' };

const filterCountries = createFilterOptions<Country>({
  stringify: (option) => `${option.name} ${option.alpha2} ${option.alpha3} ${option.region}`,
});

interface FormState {
  nickname: string;
  useNickname: boolean;
  firstName: string;
  lastName: string;
  countryCode: string;
  timezone: string;
  language: string;
  startOfWeek: DayOfWeek;
  dateFormat: string;
  timeFormat: string;
}

function accountToForm(a: Account): FormState {
  return {
    nickname: a.nickname,
    useNickname: a.useNickname,
    firstName: a.firstName ?? '',
    lastName: a.lastName ?? '',
    countryCode: a.countryCode ?? '',
    timezone: a.timezone,
    language: a.language,
    startOfWeek: a.startOfWeek,
    dateFormat: a.dateFormat,
    timeFormat: a.timeFormat,
  };
}

export function ProfilePage() {
  const { t } = useTranslation();
  const {
    data: account,
    isLoading: accountLoading,
    isError: accountError,
    error: accountErrorValue,
    refetch: refetchAccount,
  } = useAccount();
  const {
    data: countries,
    isPending: countriesPending,
    isError: countriesError,
    error: countriesErrorValue,
    refetch: refetchCountries,
  } = useCountries();
  const {
    data: timezones,
    isPending: timezonesPending,
    isError: timezonesError,
    error: timezonesErrorValue,
    refetch: refetchTimezones,
  } = useTimezones();
  const countriesList = countries ?? [];
  const timezonesList = timezones ?? [];
  const updateAccount = useUpdateAccount();
  const { showSnackbar } = useSnackbar();

  const [form, setForm] = useState<FormState | null>(null);
  const [errors, setErrors] = useState<Partial<Record<keyof FormState, string>>>({});

  const currentForm = form ?? (account ? accountToForm(account) : null);

  const timezoneValue = currentForm
    ? timezonesList.find((tz) => tz.id === currentForm.timezone) ?? null
    : null;

  const countryValue =
    currentForm && !countriesPending
      ? countriesList.find((c) => c.alpha2 === currentForm.countryCode) ?? null
      : null;

  if (accountLoading) {
    return <LoadingScreen />;
  }

  if (accountError) {
    return (
      <ErrorScreen
        title="Unable to load profile"
        message={formatApiError(accountErrorValue)}
        actionLabel="Try again"
        onAction={() => void refetchAccount()}
      />
    );
  }

  if (countriesError) {
    return (
      <ErrorScreen
        title="Unable to load countries"
        message={formatApiError(countriesErrorValue)}
        actionLabel="Try again"
        onAction={() => void refetchCountries()}
      />
    );
  }

  if (timezonesError) {
    return (
      <ErrorScreen
        title="Unable to load time zones"
        message={formatApiError(timezonesErrorValue)}
        actionLabel="Try again"
        onAction={() => void refetchTimezones()}
      />
    );
  }

  if (!account || !currentForm) {
    return (
      <ErrorScreen
        title="Unable to load profile"
        message="The profile response was empty."
        actionLabel="Reload page"
        onAction={() => window.location.reload()}
      />
    );
  }

  const update = (patch: Partial<FormState>) => {
    setForm((prev) => ({ ...(prev ?? accountToForm(account)), ...patch }));
  };

  const validate = (): boolean => {
    const e: Partial<Record<keyof FormState, string>> = {};
    if (!currentForm.nickname || currentForm.nickname.length < 3)
      e.nickname = t('profile.validation.nicknameMin');
    if (currentForm.nickname.length > 64)
      e.nickname = t('profile.validation.nicknameMax');
    if (currentForm.firstName && currentForm.firstName.length > 128)
      e.firstName = t('profile.validation.firstNameMax');
    if (currentForm.lastName && currentForm.lastName.length > 128)
      e.lastName = t('profile.validation.lastNameMax');
    if (!currentForm.language) e.language = t('profile.validation.languageRequired');
    if (!currentForm.countryCode?.trim())
      e.countryCode = t('profile.validation.countryRequired');
    if (!currentForm.timezone?.trim())
      e.timezone = t('profile.validation.timezoneRequired');
    if (!currentForm.dateFormat) e.dateFormat = t('profile.validation.dateFormatRequired');
    if (!currentForm.timeFormat) e.timeFormat = t('profile.validation.timeFormatRequired');
    setErrors(e);
    return Object.keys(e).length === 0;
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    if (!validate()) return;

    const request: UpdateRequest = {
      nickname: currentForm.nickname,
      firstName: currentForm.firstName || null,
      lastName: currentForm.lastName || null,
      useNickname: currentForm.useNickname,
      language: currentForm.language,
      countryCode: currentForm.countryCode.trim(),
      timeZone: currentForm.timezone,
      dateFormat: currentForm.dateFormat,
      timeFormat: currentForm.timeFormat,
      startOfWeek: currentForm.startOfWeek,
    };

    updateAccount.mutate(request, {
      onSuccess: (data) => {
        setForm(accountToForm(data));
        showSnackbar(t('profile.updated'), 'success');
      },
      onError: (err) => {
        showSnackbar(apiErrorSnackbarPayload(err), 'error');
      },
    });
  };

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
      {/* Page header */}
      <Box>
        <Typography variant="h6" fontWeight={500} sx={{ fontSize: { xs: 16, md: 18 } }}>
          {t('profile.title')}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {t('profile.subtitle')}
        </Typography>
      </Box>

      <form onSubmit={handleSubmit}>
        <Card sx={{ mb: 3, bgcolor: 'transparent' }}>
          <CardContent sx={{ px: 0 }}>
            <Typography variant="subtitle1" fontWeight={600} sx={{ mb: 2 }}>
              {t('profile.personalInfo')}
            </Typography>
            <Box
              sx={{
                display: 'grid',
                gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' },
                gap: 2,
              }}
            >
              <TextField
                fullWidth
                label={t('profile.nickname')}
                value={currentForm.nickname}
                onChange={(e) => update({ nickname: e.target.value })}
                error={Boolean(errors.nickname)}
                helperText={errors.nickname}
              />
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={currentForm.useNickname}
                      onChange={(e) => update({ useNickname: e.target.checked })}
                      color="primary"
                    />
                  }
                  label={t('profile.useNickname')}
                />
              </Box>
              <TextField
                fullWidth
                label={t('profile.firstName')}
                value={currentForm.firstName}
                onChange={(e) => update({ firstName: e.target.value })}
                error={Boolean(errors.firstName)}
                helperText={errors.firstName}
              />
              <TextField
                fullWidth
                label={t('profile.lastName')}
                value={currentForm.lastName}
                onChange={(e) => update({ lastName: e.target.value })}
                error={Boolean(errors.lastName)}
                helperText={errors.lastName}
              />
            </Box>

            <Typography variant="subtitle1" fontWeight={600} sx={{ mt: 4, mb: 2 }}>
              {t('profile.preferences')}
            </Typography>
            <Box
              sx={{
                display: 'grid',
                gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' },
                gap: 2,
              }}
            >
              <Autocomplete
                options={countriesList}
                disabled={countriesPending}
                value={countriesPending ? null : countryValue}
                onChange={(_, v) => update({ countryCode: v?.alpha2 ?? '' })}
                getOptionLabel={(c: Country) => c.name}
                filterOptions={filterCountries}
                renderOption={(props, option) => (
                  <Box component="li" {...props} key={option.alpha2} sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    <img
                      src={getCountryFlag(option.alpha2)}
                      alt=""
                      width={26}
                      height={15}
                      style={{ flexShrink: 0 }}
                    />
                    <span>{option.name}</span>
                  </Box>
                )}
                renderInput={(params) => {
                  const { InputProps, inputProps, InputLabelProps, ...rest } = params;
                  return (
                    <TextField
                      {...rest}
                      label={t('profile.country')}
                      error={Boolean(errors.countryCode)}
                      helperText={errors.countryCode}
                      slotProps={{
                        inputLabel: InputLabelProps,
                        input: {
                          ...InputProps,
                          endAdornment: (
                            <>
                              {countriesPending ? (
                                <CircularProgress color="inherit" size={20} sx={{ mr: 0.5 }} />
                              ) : null}
                              {InputProps.endAdornment}
                            </>
                          ),
                        },
                        htmlInput: inputProps,
                      }}
                      aria-busy={countriesPending}
                    />
                  );
                }}
                isOptionEqualToValue={(a, b) => a.alpha2 === b.alpha2}
              />
              <Autocomplete
                options={timezonesList}
                disabled={timezonesPending}
                value={timezonesPending ? null : timezoneValue}
                onChange={(_, v) => update({ timezone: v?.id ?? '' })}
                getOptionLabel={(opt: Timezone) => formatTimezone(opt)}
                renderInput={(params) => {
                  const { InputProps, inputProps, InputLabelProps, ...rest } = params;
                  return (
                    <TextField
                      {...rest}
                      label={t('profile.timezone')}
                      error={Boolean(errors.timezone)}
                      helperText={errors.timezone}
                      slotProps={{
                        inputLabel: InputLabelProps,
                        input: {
                          ...InputProps,
                          endAdornment: (
                            <>
                              {timezonesPending ? (
                                <CircularProgress color="inherit" size={20} sx={{ mr: 0.5 }} />
                              ) : null}
                              {InputProps.endAdornment}
                            </>
                          ),
                        },
                        htmlInput: inputProps,
                      }}
                      aria-busy={timezonesPending}
                    />
                  );
                }}
                isOptionEqualToValue={(opt, val) => opt.id === val.id}
              />
              <TextField
                fullWidth
                select
                label={t('profile.language')}
                value={currentForm.language}
                onChange={(e) => update({ language: e.target.value })}
                error={Boolean(errors.language)}
                helperText={errors.language}
              >
                {Object.entries(LANGUAGES).map(([code, name]) => (
                  <MenuItem key={code} value={code}>
                    {name}
                  </MenuItem>
                ))}
              </TextField>
              <TextField
                fullWidth
                select
                label={t('profile.startOfWeek')}
                value={currentForm.startOfWeek}
                onChange={(e) => update({ startOfWeek: e.target.value as DayOfWeek })}
              >
                {DAYS_OF_WEEK.map((day) => (
                  <MenuItem key={day} value={day}>
                    {t(`days.${day}`)}
                  </MenuItem>
                ))}
              </TextField>
              <TextField
                fullWidth
                select
                label={t('profile.dateFormat')}
                value={currentForm.dateFormat}
                onChange={(e) => update({ dateFormat: e.target.value })}
                error={Boolean(errors.dateFormat)}
                helperText={errors.dateFormat}
              >
                {DATE_FORMATS.map((fmt) => (
                  <MenuItem key={fmt} value={fmt}>
                    {fmt} ({formatDateExample(fmt)})
                  </MenuItem>
                ))}
              </TextField>
              <TextField
                fullWidth
                select
                label={t('profile.timeFormat')}
                value={currentForm.timeFormat}
                onChange={(e) => update({ timeFormat: e.target.value })}
                error={Boolean(errors.timeFormat)}
                helperText={errors.timeFormat}
              >
                {TIME_FORMATS.map((fmt) => (
                  <MenuItem key={fmt} value={fmt}>
                    {fmt} ({formatDateExample(fmt)})
                  </MenuItem>
                ))}
              </TextField>
            </Box>

            <Box sx={{ mt: 3, display: 'flex', justifyContent: 'flex-end' }}>
              <LoadingButton
                loading={updateAccount.isPending}
                type="submit"
                variant="contained"
                size="medium"
              >
                {t('common.save')}
              </LoadingButton>
            </Box>
          </CardContent>
        </Card>
      </form>
    </Box>
  );
}
