# CrystalId
Minimalist Id generator inspired by Twitter Snowflake, implemented in F#.

Caller must maintain state and pass into the generator.
Caller passes in previous CrystalId as a seed, together with current time, and receives back a new CrystalId (or an exception).
Twitter Snowflake places the timestamp in the leftmost bits, followed by the process number and then the sequence number.

CrystalId places the timestamp in the most leftmost bits, followed by the sequence number, then the process number.
This way a process can keep on generating sequences overflowing into the timestamp, effectively borrowing IDs against the future.
This is safe as long as if the process loses its state, it cannot restart within the time span covered by the 'borrowed' IDs.

For example, an ID which is 53 bits long could use:
 - 31 bits for the number of seconds since 2020-01-01 (spanning 68 years)
 - 12 bits for 4096 IDs per second
 - 10 bits for 1024 processes

The resulting code fits in 11 base32 characters (see Crockford's base 32), and is Javascript integer compatible, FWIW.

Future expansion can be handled by shifting left.
This should work without causing any collisions, provided that the shift to the left is large enough.
Care would have to be taken.
Alternatively the most significant bit(s) can be used to version the IDs and prevent collisions.

Todo: 
 - maybe return some CrystalId or Error instead of throwing exception when rate is exceeded.
 - Disallow passing in a date before the start of epoch
