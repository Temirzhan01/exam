const checkResAndRunCheckCond = provisions?.length > 0
  ? (provisions.some(x => x?.runCheck)
      ? provisions.every(x => utils.validate(x?.checkResult))
      : !provisions.some(x => !x?.runCheck && x?.pledgerBIN !== collateralDTO?.bin))
  : true;
