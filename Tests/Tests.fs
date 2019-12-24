module Tests

open NUnit.Framework
open System
open CrystalId

[<TestFixture>]
type TestClass () =
  [<Test>]
  member this.ReturnsId() = 

    let CrystalId = {
      epochStartYear = 2019
      sequenceBitCount = 2
      processBitCount = 10
      processNumber = 1L
      maximumBorrowedIdCount = 1L
      epochOffsetSeconds = 0L
      sequenceNumber = 0L
      id = 0L }

    let CrystalId2 = nextId CrystalId (DateTime(2019, 01, 01, 00, 00, 00))

    Assert.True(CrystalId2.id > 0L)


  [<Test>]
  member this.IdIsMonotonic() =
    let CrystalId = {
      epochStartYear = 2019
      sequenceBitCount = 2
      processBitCount = 10
      processNumber = 1L
      maximumBorrowedIdCount = 1L
      epochOffsetSeconds = 0L
      sequenceNumber = 0L
      id = 0L }

    let CrystalId2 = nextId CrystalId (DateTime(2019, 01, 01, 00, 00, 30))
    let CrystalId3 = nextId CrystalId2 (DateTime(2019, 01, 01, 00, 00, 29)) // go back in time

    Assert.True(CrystalId3.id > CrystalId2.id) // but id must increase

  [<Test>]
  member this.SequenceNumberIncreasesWhenTimeStatic() =
    let CrystalId = {
      epochStartYear = 2019
      sequenceBitCount = 2
      processBitCount = 10
      processNumber = 1L
      maximumBorrowedIdCount = 1L
      epochOffsetSeconds = 0L
      sequenceNumber = 0L
      id = 0L }

    let CrystalId2 = nextId CrystalId (DateTime(2019, 01, 01, 00, 00, 30))
    let CrystalId3 = nextId CrystalId2 (DateTime(2019, 01, 01, 00, 00, 30)) // time static

    Assert.True(CrystalId3.sequenceNumber > CrystalId2.sequenceNumber) // sequence must increase
    Assert.True(CrystalId3.id > CrystalId2.id) // id must increase

  [<Test>]
  member this.SequenceIncreasesWhenTimeReverses() =
    let CrystalId = {
      epochStartYear = 2019
      sequenceBitCount = 2
      processBitCount = 10
      processNumber = 1L
      maximumBorrowedIdCount = 1L
      epochOffsetSeconds = 0L
      sequenceNumber = 0L
      id = 0L }

    let CrystalId2 = nextId CrystalId (DateTime(2019, 01, 01, 00, 00, 30))
    let CrystalId3 = nextId CrystalId2 (DateTime(2019, 01, 01, 00, 00, 29)) // go back in time

    Assert.True(CrystalId3.sequenceNumber - CrystalId2.sequenceNumber = 1L) // sequence must increase
    Assert.True(CrystalId3.id > CrystalId2.id) // id must increase


  [<Test>]
  member this.SequenceZeroWhenTimeIncreases() =
    let CrystalId = {
      epochStartYear = 2019
      sequenceBitCount = 2
      processBitCount = 10
      processNumber = 1L
      maximumBorrowedIdCount = 1L
      epochOffsetSeconds = 0L
      sequenceNumber = 0L
      id = 0L }

    let CrystalId2 = nextId CrystalId (DateTime(2019, 01, 01, 00, 00, 30))
    let CrystalId3 = nextId CrystalId2 (DateTime(2019, 01, 01, 00, 00, 31))

    Assert.True(CrystalId3.sequenceNumber = 0L)
    Assert.True(CrystalId3.id > CrystalId2.id) // id must increase

  [<Test>]
  member this.IdShiftCorrect() =
    let CrystalId = {
      epochStartYear = 2019
      sequenceBitCount = 2
      processBitCount = 10
      processNumber = 1L
      maximumBorrowedIdCount = 1L
      epochOffsetSeconds = 0L
      sequenceNumber = 0L
      id = 0L }

    let CrystalId2 = nextId CrystalId (DateTime(2019, 01, 01, 00, 00, 30))
    let CrystalId3 = nextId CrystalId2 (DateTime(2019, 01, 01, 00, 00, 31))

    Assert.True((CrystalId3.id - CrystalId2.id) = ((CrystalId3.epochOffsetSeconds - CrystalId2.epochOffsetSeconds) <<< (CrystalId2.sequenceBitCount + CrystalId2.processBitCount))) // id must increase

