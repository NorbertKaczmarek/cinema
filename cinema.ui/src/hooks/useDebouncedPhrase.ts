import { useCallback, useEffect, useState } from 'react';

import { debounce } from 'lodash';

import { MIN_SEARCH_PHRASE } from 'Constants/index';
import { useIsMounted } from 'Hooks/useIsMounted';

const useDebouncedPhrase = (
  minSearchPhrase = MIN_SEARCH_PHRASE,
  customUpdateFn?: (value?: string) => void
) => {
  const [phrase, setPhrase] = useState<string | undefined>();
  const isMounted = useIsMounted();

  const debouncedOnSearch = useCallback(
    debounce((val: string) => {
      if (!isMounted()) return;
      if (val.length < minSearchPhrase) {
        setPhrase(undefined);
        customUpdateFn?.(undefined);
      } else {
        setPhrase(val);
        customUpdateFn?.(val);
      }
    }, 300),
    []
  );

  const onSearch = (val: string) => {
    debouncedOnSearch(val);
  };

  useEffect(() => {
    return debouncedOnSearch.cancel();
  }, []);

  return { debouncedPhrase: phrase, onSearch };
};

export default useDebouncedPhrase;
