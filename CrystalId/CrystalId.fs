module CrystalId

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


let private exampleUse =

  let mutable CrystalId = {
    epochStartYear = 2019
    sequenceBitCount = 12
    processBitCount = 10
    processNumber = 1L
    maximumBorrowedIdCount = 4096L * 10L // allow system to borrow up to 10 seconds worth of IDs
    epochOffsetSeconds = 0L
    sequenceNumber = 0L
    id = 0L
  }

  CrystalId <- nextId CrystalId (DateTime.UtcNow);
  printfn "%A" CrystalId

  CrystalId <- nextId CrystalId (DateTime.UtcNow);
  printfn "%A" CrystalId


