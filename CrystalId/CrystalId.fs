module CrystalId

(*
Minimalist stateless Id generator inspired by Twitter snowflake.
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

Todo: maybe return some CrystalId or Error instead of throwing exception when rate is exceeded.
*)

open System

type CrystalId = {
  epochStartYear : int ;
  sequenceBitCount : int;
  processBitCount : int;
  processNumber : int64;
  maximumBorrowedIdCount : int64
  epochOffsetSeconds : int64;
  sequenceNumber : int64;
  id : int64;
}

let private secondsFromStartOfEpoch (now: DateTime) (startOfEpoch: DateTime) = 
  int64 (now - startOfEpoch).TotalSeconds

let private calculateId sequenceBitCount processBitCount processNumber secondsFromStartOfEpoch sequenceNumber =
  (secondsFromStartOfEpoch <<< (processBitCount + sequenceBitCount))
  + (sequenceNumber <<< processBitCount)
  + processNumber

let nextId seed now =
  let currentEpochOffsetSeconds = secondsFromStartOfEpoch now (DateTime (seed.epochStartYear, 01, 01, 00, 00, 00)) 
  let thisEpochOffsetSeconds = [|currentEpochOffsetSeconds; seed.epochOffsetSeconds |] |> Seq.max  // make clock monotonic
  let thisSequenceNumber = 
    if (calculateId seed.sequenceBitCount seed.processBitCount seed.processNumber thisEpochOffsetSeconds 0L) > seed.id then 0L // set sequence back to zero if time has advanced
    elif seed.sequenceNumber < (1L <<< seed.sequenceBitCount) + seed.maximumBorrowedIdCount - 1L then seed.sequenceNumber + 1L // increase sequence if necessary and possible
    else failwith "Exceeded rate limit, wait before requesting more IDs"

  { seed with 
      epochOffsetSeconds = thisEpochOffsetSeconds
      sequenceNumber = thisSequenceNumber
      id = calculateId seed.sequenceBitCount seed.processBitCount seed.processNumber thisEpochOffsetSeconds thisSequenceNumber }


let private example =

  let mutable CrystalId = {
    epochStartYear = 2019
    sequenceBitCount = 12
    processBitCount = 10
    processNumber = 1L
    maximumBorrowedIdCount = 4096L * 10L // allow system to borrow up to 10 seconds worth of IDs
    epochOffsetSeconds = 0L // initial value
    sequenceNumber = 0L
    id = 0L
  }

  CrystalId <- nextId CrystalId (DateTime.UtcNow);
  printfn "%A" CrystalId

  CrystalId <- nextId CrystalId (DateTime.UtcNow);
  printfn "%A" CrystalId


let private test = 

  let mutable CrystalId = {
    epochStartYear = 2019
    sequenceBitCount = 2
    processBitCount = 10
    processNumber = 1L
    maximumBorrowedIdCount = 1L
    epochOffsetSeconds = 0L
    sequenceNumber = 0L
    id = 0L
  }

  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,01));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,02));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,02));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,02));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
  CrystalId <- nextId CrystalId (DateTime (2020,02,02,00,00,00));
  printfn "%A" CrystalId
