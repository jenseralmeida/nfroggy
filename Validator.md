# Resume #

This API pretend to be a simple but powerfull library for general validation. It was build from the ground up to be extensible, so the sintax was created to be natural with native and with extension validation.


# Examples #

## Simple System.Type validation ##
```
    bool isValid = Validation<int>.Create().IsValid("14")
```

## Simple Comparable validation (interval of values) ##
```
    Validation<int> validation = Validation<int>.Create()
        .SetUpComparable(5);

    Assert.IsTrue(validation.IsValid(5));
```